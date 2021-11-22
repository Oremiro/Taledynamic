import axios from "axios";
import { ActionTree } from "vuex";
import { VueCookieNext } from "vue-cookie-next";
import { UserApi } from "@/helpers/api/user";
import { SignUpFormData, SignInFormData } from "@/models";
import {
  State,
  UserState,
  UpdatedEmailData,
  UpdatedPasswordData,
  User
} from "@/models/store";

export const actions: ActionTree<UserState, State> = {
  async init({ dispatch }): Promise<boolean> {
    const isRemembered: string | null = VueCookieNext.getCookie("remembered");
    const localStorageUser: string | null = localStorage.getItem("user");
    try {
      if (isRemembered === "1" && localStorageUser) {
        await dispatch("refresh");
        return true;
      } else {
        throw new Error("Cookie/storage is empty");
      }
    } catch (error) {
      VueCookieNext.removeCookie("remembered");
      localStorage.removeItem("user");
    }
    return false;
  },
  async register(_context, formData: SignUpFormData): Promise<void> {
    const newUser = {
      email: formData.email.value,
      password: formData.password.value,
      confirmPassword: formData.confirmedPassword.value
    };
    try {
      const { data } = await UserApi.create(newUser);
      if (data.statusCode === 200) {
        return;
      } else {
        throw new Error("Ошибка регистрации");
      }
    } catch (error) {
      if (axios.isAxiosError(error)) {
        if (error.response?.status == 400) {
          throw new Error(
            "Пользователь с таким почтовым адресом уже существует"
          );
        } else {
          throw new Error("Ошибка регистрации");
        }
      }
    }
  },
  async login({ commit }, formData: SignInFormData): Promise<UserState> {
    const requestedUser = {
      email: formData.email.value,
      password: formData.password.value
    };
    try {
      const { data } = await UserApi.authenticate(requestedUser);
      if (data.statusCode === 200) {
        const responsedUser: User = {
          id: data.id ?? null,
          email: data.email ?? formData.email.value
        };
        const accessToken: string = data.jwtToken ?? "";
        commit("login", {
          user: responsedUser,
          accessToken: accessToken
        });
        const expireValue: string = formData.remembered.value ? "7d" : "0";
        VueCookieNext.setCookie("remembered", "1", { expire: expireValue });
        localStorage.setItem("user", JSON.stringify(responsedUser));
        return { user: responsedUser, accessTokenInMemory: accessToken };
      } else {
        throw new Error("Ошибка авторизации");
      }
    } catch (error) {
      if (axios.isAxiosError(error)) {
        if (error.response?.status === 404) {
          throw new Error("Пользователь с такими данными не найден");
        } else {
          throw new Error("Ошибка авторизации");
        }
      } else {
        throw error;
      }
    }
  },
  async logout({ commit, state }): Promise<void> {
    try {
      const { data } = await UserApi.revokeToken({}, state.accessTokenInMemory);
      if (data.isSuccess && data.statusCode === 200) {
        commit("logout");
        VueCookieNext.removeCookie("remembered");
        localStorage.removeItem("user");
        return;
      } else {
        throw new Error("Ошибка при выходе из аккаунта");
      }
    } catch (error) {
      if (axios.isAxiosError(error)) {
        if (error.response?.status === 404) {
          throw new Error("Пользователь с таким токеном не найден");
        } else {
          throw new Error("Ошибка при обнулении токена");
        }
      }
    }
  },
  async refresh({ commit }): Promise<void> {
    try {
      const { data } = await UserApi.refreshToken();
      if (data.id && data.email && data.jwtToken) {
        const user: User = { id: data.id, email: data.email };
        const token: string = data.jwtToken;
        commit("login", {
          user: user,
          accessToken: token
        });
        localStorage.setItem("user", JSON.stringify(user));
        return;
      } else {
        throw new Error("Непредвиденная ошибка");
      }
    } catch (error) {
      if (axios.isAxiosError(error)) {
        if (error.response?.status === 404) {
          throw new Error("Сессия устарела");
        } else {
          throw new Error("Ошибка обновления токена");
        }
      }
    }
  },
  async updateEmail({ commit, state }, data: UpdatedEmailData): Promise<void> {
    if (state.user.id) {
      try {
        const { data: responseData } = await UserApi.update(
          {
            id: state.user.id,
            password: data.currentPassword,
            email: data.newEmail
          },
          state.accessTokenInMemory
        );
        const user: User = {
          id: responseData.user.id,
          email: responseData.user.email
        };
        commit("setUser", { user });
        localStorage.setItem("user", JSON.stringify(user));
        return;
      } catch (error) {
        if (axios.isAxiosError(error)) {
          if (error.response?.status === 404) {
            throw new Error("Неправильный текущий пароль");
          } else if (error.response?.status === 400) {
            throw new Error("Данный email занят другим пользователем");
          } else {
            throw new Error(error.message);
          }
        }
      }
    } else {
      throw new Error("Отсутствует ID пользователя");
    }
  },
  async updatePassword(
    { commit, state },
    data: UpdatedPasswordData
  ): Promise<void> {
    if (state.user.id) {
      try {
        const { data: responseData } = await UserApi.update(
          {
            id: state.user.id,
            password: data.currentPassword,
            newPassword: data.newPassword,
            confirmNewPassword: data.confirmedNewPassword
          },
          state.accessTokenInMemory
        );
        const user: User = {
          id: responseData.user.id,
          email: responseData.user.email
        };
        commit("setUser", { user });
        localStorage.setItem("user", JSON.stringify(user));
        return;
      } catch (error) {
        if (axios.isAxiosError(error)) {
          if (error.response?.status === 404) {
            throw new Error("Неправильный текущий пароль");
          } else if (error.response?.status === 400) {
            throw new Error("Пароли не совпадают");
          } else {
            throw new Error(error.message);
          }
        }
      }
    } else {
      throw new Error("Отсутствует ID пользователя");
    }
  },
  async delete({ commit, state }): Promise<void> {
    if (state.user.id) {
      try {
        await UserApi.delete(
          { userId: state.user.id },
          state.accessTokenInMemory
        );
        commit("logout");
        VueCookieNext.removeCookie("remembered");
        localStorage.removeItem("user");
        return;
      } catch (error) {
        if (axios.isAxiosError(error)) {
          if (error.response?.status === 404) {
            throw new Error("?");
          } else if (error.response?.status === 400) {
            throw new Error("Неверный ID пользователя");
          } else {
            throw new Error(error.message);
          }
        }
      }
      return;
    } else {
      throw new Error("Отсутствует ID пользователя");
    }
  }
};

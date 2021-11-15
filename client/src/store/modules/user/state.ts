import { UserState } from "@/interfaces/store";

export const state: UserState = {
  user: {
    id: null,
    email: "",
  },
  accessTokenInMemory: "",
};

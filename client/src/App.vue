<template>
  <n-config-provider :theme="currentTheme">
    <n-global-style />
    <n-message-provider>
      <n-layout style="min-height: 100vh" position="absolute">
        <n-loading-bar-provider>
          <layout-header :current-theme="currentTheme" @change-theme="setTheme" />
        </n-loading-bar-provider>
        <n-layout position="absolute" style="top: 3.3rem;" :native-scrollbar="false">
          <n-layout-content embedded content-style="min-height: calc(100vh - 3.3rem)">
            <router-view v-slot="{ Component }">
              <transition name="fade" mode="out-in">
                <component :is="Component" />
              </transition>
            </router-view>
          </n-layout-content>
          <layout-footer />
        </n-layout>
      </n-layout>
    </n-message-provider>
  </n-config-provider>
</template>

<script setup lang="ts">
import "vfonts/OpenSans.css";
import { ref, onUnmounted } from "vue";
import { useRouter } from "vue-router";
import { darkTheme, useOsTheme } from "naive-ui";
import LayoutHeader from "@/layouts/LayoutHeader.vue";
import LayoutFooter from "@/layouts/LayoutFooter.vue";
import { Theme } from "@/models";
import { useStore } from "@/store";
import { LoginState } from "@/models/store";

const currentTheme = ref<Theme>(darkTheme);
const storedTheme: string | null = localStorage.getItem("theme");

if (storedTheme) {
  currentTheme.value = storedTheme === "dark" ? darkTheme : null;
} else {
  const osThemeRef = useOsTheme();
  currentTheme.value = osThemeRef.value === "dark" ? darkTheme : null;
}

function setTheme(value: Theme): void {
  currentTheme.value = value;
  localStorage.setItem("theme", value === null ? "light" : "dark");
}

const router = useRouter();
const store = useStore();

onstorage = (event: StorageEvent): void => {
  if (event.storageArea === localStorage) {
    if (event.key === "theme") {
      currentTheme.value = event.newValue === "dark" ? darkTheme : null;
    } else if (event.key === "user" && event.newValue === null) {
      store.commit("user/logout");
      router.push({ name: "AuthSignIn" });
    }
  }
};

const signinBC = new BroadcastChannel("signin");
signinBC.onmessage = (ev: MessageEvent<LoginState>): void => {
  if (!store.getters["user/isLoggedIn"]) {
    const userState = ev.data;
    store.commit("user/login", {
      user: userState.user,
      accessToken: userState.accessTokenInMemory
    });
    if (router.currentRoute.value.name === "AuthSignIn" || router.currentRoute.value.name === "AuthSignUp") {
      router.push({ name: "ProfileIndex" });
    }
  }
};
onUnmounted(() => {
  signinBC.close();
});
</script>

<style lang="scss">
#app {
  font-family: v-sans;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
}

.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.3s ease-out;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}

.container {
  display: flex;
  justify-content: center;
  padding: 2.5rem 3.5rem;
}
</style>

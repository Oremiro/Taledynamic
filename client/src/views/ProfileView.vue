<template>
  <div class="container">
    <n-grid :cols="30" style="max-width: 40rem">
      <n-gi :span="8">
        <n-card hoverable content-style="padding-left: 0; padding-right: 0;">
          <div style="padding: 0 2rem;">
            <n-h4 style="margin-bottom: .9rem;">
              <n-text type="primary">Аккаунт</n-text>
            </n-h4>
          </div>
          <n-menu :options="menuOptions" :value="route.path" style="padding: 0 0.8rem" />
        </n-card>
      </n-gi>
      <n-gi :span="1" />
      <n-gi :span="21">
        <router-view v-slot="{ Component }">
          <transition name="fade" mode="out-in">
            <n-card :key="route.path" hoverable>
              <component :is="Component" />
            </n-card>
          </transition>
        </router-view>
      </n-gi>
    </n-grid>
  </div>
</template>

<script setup lang="ts">
import { useStore } from "@/store";
import { MenuOption, useMessage } from "naive-ui";
import { h } from "vue";
import { RouterLink, useRoute, useRouter } from "vue-router";

const store = useStore();
const router = useRouter();
const route = useRoute();
const message = useMessage();
const menuOptions: MenuOption[] = [
  {
    label: () => h(RouterLink, { to: "/account/data" }, { default: () => "Данные" }),
    key: "/account/data"
  },
  {
    label: () => h(RouterLink, { to: "/account/settings" }, { default: () => "Настройки" }),
    key: "/account/settings"
  },
  {
    onClick: async (): Promise<void> => {
      try {
        await store.dispatch("user/logout");
        router.push({ name: "Auth" });
        message.info("Вы вышли из аккаунта");
      } catch (error) {
        if (error instanceof Error) {
          message.error(error.message);
        }
      }
    },
    label: "Выйти",
    key: "/account/quit"
  }
];
</script>

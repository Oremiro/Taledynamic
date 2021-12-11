<template>
  <div class="container">
    <n-card title="Подключение аккаунта Telegram" style="max-width: max-content">
      <div v-if="store.getters['user/isLoggedIn']">
        <div style="margin-bottom: 2rem">
          Подключив аккаунт Telegram, Вы сможете взаимодействовать с сервисом Taledynamic через бота.
        </div>
        <n-divider />
        <div style="margin-bottom: 1rem">Подключить аккаунт?</div>
        <div style="display: flex; flex-wrap: wrap; gap: 1rem">
          <n-button type="primary" ghost>Подключить</n-button>
          <n-button type="error" ghost @click="router.push({ name: 'Main' })">Отменить</n-button>
        </div>
      </div>
      <div v-else>
        <div style="margin-bottom: 2rem">
          Для подключения необходимо войти в свой аккаунт или зарегистрировать новый.
        </div>
        <n-divider />
        <div style="display: flex; flex-wrap: wrap; gap: 1rem">
          <n-button type="primary" ghost @click="router.push({ name: 'AuthSignIn' })">Войти</n-button>
          <n-button ghost @click="router.push({ name: 'AuthSignUp' })">Зарегистрироваться</n-button>
        </div>
      </div>
    </n-card>
  </div>
</template>

<script setup lang="ts">
import { NDivider } from "naive-ui";
import { useRouter } from "vue-router";
import { useStore } from "@/store";

const store = useStore();
const router = useRouter();
</script>

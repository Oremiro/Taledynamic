<template>
  <div class="container">
    <n-card title="Подключение аккаунта Telegram" style="max-width: max-content">
      <div v-if="store.getters['user/isLoggedIn']">
        <div style="margin-bottom: 2rem">
          Подключив аккаунт Telegram, Вы сможете взаимодействовать с сервисом Taledynamic через бота.
        </div>
        <n-divider />
        <div v-if="!isNaN(telegramId)">
          <div style="margin-bottom: 1rem">
            <n-text v-if="telegramName">Подключить аккаунт <n-text type="primary">{{ telegramName }}</n-text>?</n-text>
            <n-text v-else>Подключить аккаунт?</n-text>
          </div>
          <div style="display: flex; flex-wrap: wrap; gap: 1rem">
            <n-button type="primary" ghost @click="bindAccount">Подключить</n-button>
            <n-button type="error" ghost @click="router.push({ name: 'Main' })">Не подключать</n-button>
          </div>
        </div>
        <div v-else>
          <div style="margin-bottom: 1rem">
            Начать процесс подключения можно в боте
            <n-a href="https://t.me/TaleDynamicBot" target="_blank" style="text-decoration: none">@TaleDynamicBot</n-a>
            , отправив команду
            <n-text type="primary">/auth</n-text>.
          </div>
          <n-button type="primary" ghost tag="a" href="https://t.me/TaleDynamicBot" target="_blank"
            >Открыть чат</n-button
          >
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
import { computed } from "vue";
import { NDivider, useMessage } from "naive-ui";
import { useRoute, useRouter } from "vue-router";
import { useStore } from "@/store";

const store = useStore();
const router = useRouter();
const route = useRoute();
const message = useMessage();

const telegramId = computed<number>(() => (typeof route.query.tgId === "string" ? parseInt(route.query.tgId) : NaN));
const telegramName = computed<string>(() => (typeof route.query.tgName === "string" ? route.query.tgName : ""));

async function bindAccount(): Promise<void> {
  try {
    console.log();
  } catch (error) {
    if (error instanceof Error) {
      message.error(error.message);
    }
  }
}
</script>

<template>
  <n-h4>
    <n-text type="primary">Интеграция Telegram </n-text>
  </n-h4>
  <div v-if="tgUserId">
    <div style="margin-bottom: 1rem">
      Подключен аккаунт с ID: <n-text type="primary">{{ tgUserId }}</n-text>
    </div>
    <dynamically-typed-button type="error" ghost @click="revokeTelegram">Отключить</dynamically-typed-button>
  </div>
  <n-button v-else @click="router.push({ name: 'IntegrationTelegram' })">Подключить</n-button>
  <n-h4>
    <n-text type="primary"> Удаление аккаунта </n-text>
  </n-h4>
  <delayed-button
    ref="submitButtonRef"
    :type="buttonType"
    ghost
    style="margin-right: 1rem"
    @click="showDeletionConfirmation"
  >
    {{ buttonText }}
  </delayed-button>
  <n-button v-show="isDeletionConfirmationShown" @click="cancelDeletion"> Нет, я передумал </n-button>
  <n-collapse-transition :show="isDeletionConfirmationShown">
    <div style="margin-top: 1rem">Вы уверены, что хотите удалить аккаунт?</div>
  </n-collapse-transition>
</template>

<script setup lang="ts">
import { onMounted, ref } from "vue";
import { useRouter } from "vue-router";
import { useMessage } from "naive-ui";
import { useStore } from "@/store";
import DelayedButton from "@/components/DelayedButton.vue";
import { IntegrationTelegramApi } from "@/helpers/api/integrations";
import DynamicallyTypedButton from "@/components/DynamicallyTypedButton.vue";
import axios from "axios";

const router = useRouter();
const store = useStore();
const message = useMessage();
const isDeletionConfirmationShown = ref<boolean>(false);
const submitButtonRef = ref<InstanceType<typeof DelayedButton>>();
const buttonText = ref<string>("Удалить");
const buttonType = ref<"default" | "error">("default");

async function deleteUser(): Promise<void> {
  try {
    await store.dispatch("user/delete");
    router.push({ name: "Auth" });
    message.info("Вы успешно удалили аккаунт");
  } catch (error) {
    if (error instanceof Error) {
      message.error(error.message);
    }
  }
}

async function showDeletionConfirmation(): Promise<void> {
  if (isDeletionConfirmationShown.value) {
    await deleteUser();
  } else {
    isDeletionConfirmationShown.value = true;
    buttonText.value = "Да";
    buttonType.value = "error";
    submitButtonRef.value?.holdDisabled();
  }
}

function cancelDeletion() {
  isDeletionConfirmationShown.value = false;
  buttonText.value = "Удалить";
  buttonType.value = "default";
  submitButtonRef.value?.cancelHolding();
}

const tgBindingId = ref<number | null>(null);
const tgUserId = ref<string | null>(null);

async function revokeTelegram(): Promise<void> {
  if (tgBindingId.value === null) return;
  try {
    await store.dispatch("user/refreshExpired");
    await IntegrationTelegramApi.revoke({ id: tgBindingId.value }, store.getters["user/accessToken"]);
    message.success("Аккаунт успешно отключен")
    tgUserId.value = null;
  } catch (error) {
    console.log(error);
  }
}

onMounted(async () => {
  try {
    await store.dispatch("user/refreshExpired");
    const { data } = await IntegrationTelegramApi.get(store.getters["user/accessToken"]);
    tgBindingId.value = data.item.id;
    tgUserId.value = data.item.tgUserId;
  } catch (error) {
    if (axios.isAxiosError(error)) {
      if (error.response?.status === 404) {
        tgBindingId.value = null;
        tgUserId.value = null;
      }
    }
    else if (error instanceof Error) {
      console.log(error.message);
    }
  }
});
</script>

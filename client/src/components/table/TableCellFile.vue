<template>
  <div style="height: 34px; position: relative">
    <div
      v-if="isFileUploaded"
      style="
        height: 100%;
        display: flex;
        align-items: center;
        justify-content: space-between;
        padding: 0 1rem;
        gap: 0.5rem;
      "
    >
      <n-button text @click="downloadFile">
        <n-ellipsis style="max-width: 10rem; line-height: initial;" :tooltip="{ delay: 500 }">
          {{ name }}
        </n-ellipsis>
      </n-button>
      <dynamically-typed-button type="error" text @click="removeFile">
        <n-icon size="1rem">
          <dismiss-icon />
        </n-icon>
      </dynamically-typed-button>
    </div>
    <n-upload
      v-else
      :show-file-list="false"
      :max="1"
      :custom-request="customRequest"
      @before-upload="onBeforeUpload"
      @finish="onFinish"
    >
      <n-upload-dragger
        class="table-cell-upload-dragger"
        @mouseenter="isTextTipShown = true"
        @mouseleave="isTextTipShown = false"
      >
        <n-text v-show="isTextTipShown" style="font-size: 0.8rem" :depth="3">Перетащите файл сюда</n-text>
      </n-upload-dragger>
    </n-upload>
  </div>
</template>

<script setup lang="ts">
import { ref, watch } from "vue";
import { NUpload, NUploadDragger, UploadCustomRequestOptions, useMessage, NEllipsis } from "naive-ui";
import { toDataURL } from "@/helpers";
import { OnBeforeUpload, OnFinish } from "@/components/table/upload";
import { DismissIcon } from "@/components/icons";
import DynamicallyTypedButton from "@/components/DynamicallyTypedButton.vue";
import { FileApi } from "@/helpers/api/file";
import { useStore } from "@/store";
import { TableDataFile } from "@/models/table";

const props = defineProps<{
  value: TableDataFile | null;
}>();

const emit = defineEmits<{
  (e: "update", value: TableDataFile | null): void;
}>();

const isTextTipShown = ref<boolean>(false);

const isFileUploaded = ref<boolean>(false);
const uId = ref<string | null>(null);
const name = ref<string>("");

watch(
  () => props.value,
  (value) => {
    if (value !== null) {
      isFileUploaded.value = true;
      uId.value = value.uId;
      name.value = value.name;
    }
  },
  { immediate: true }
);

const store = useStore();

async function customRequest({ file, onFinish, onError }: UploadCustomRequestOptions) {
  if (file.file === undefined || file.file === null) {
    onError();
  } else {
    try {
      const fileBase64 = await toDataURL(file.file);
      await store.dispatch("user/refreshExpired");
      const { data } = await FileApi.create(
        { fileBase64: fileBase64, type: file.file.type },
        store.getters["user/accessToken"]
      );
      uId.value = data.item.id;
      name.value = file.name;
      onFinish();
    } catch (error) {
      console.log(error);
      onError();
    }
  }
}

const message = useMessage();

const onBeforeUpload: OnBeforeUpload = async ({ file }) => {
  if (file.file === null || file.file === undefined) return false;
  if (file.file.size === undefined || file.file.size > 10 * 1024 * 1024) {
    message.error("Размер файла не должен превышать 10 Мб");
    return false;
  }
  if (file.file.name.length > 100) {
    message.error("Название файла не должно быть длиннее 100 символов");
    return false;
  }
  if (file.file.type === "") {
    message.error("Некорректный тип файла");
    return false;
  }
  return true;
};

const onFinish: OnFinish = ({ file }) => {
  isFileUploaded.value = true;
  emit("update", { uId: uId.value, name: name.value });
  return file;
};

async function downloadFile(): Promise<void> {
  if (uId.value === null) return;
  try {
    await store.dispatch("user/refreshExpired");
    const { data } = await FileApi.getLink({ uId: uId.value }, store.getters["user/accessToken"]);
    await downloadUsingFetch(data.item.base64String, name.value);
  } catch (error) {
    if (error instanceof Error) {
      console.log(error);
    }
  }
}

async function downloadUsingFetch(dataURL: string, filename: string) {
  const res = await fetch(dataURL);
  const fileBlob = await res.blob();
  const fileURL = URL.createObjectURL(fileBlob);
  const anchor = document.createElement("a");
  anchor.href = fileURL;
  anchor.download = filename;
  anchor.click();
  URL.revokeObjectURL(fileURL);
}

async function removeFile() {
  isFileUploaded.value = false;
  uId.value = null;
  name.value = "";
  emit("update", null);
}
</script>

<style scoped lang="scss">
@import "@/components/table/style.scss";
</style>

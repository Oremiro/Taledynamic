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
      <n-button text @click="downloadFile">{{ fileList[0].name }}</n-button>
      <dynamically-typed-button type="error" text>
        <n-icon size="1rem">
          <dismiss-icon />
        </n-icon>
      </dynamically-typed-button>
    </div>
    <n-upload
      v-else
      v-model:file-list="fileList"
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
import { ref } from "vue";
import { NUpload, NUploadDragger, UploadCustomRequestOptions, UploadFileInfo, useMessage } from "naive-ui";
import { toBase64 } from "@/helpers";
import { OnBeforeUpload, OnFinish } from "@/components/table/upload";
import { DismissIcon } from "@/components/icons";
import DynamicallyTypedButton from "@/components/DynamicallyTypedButton.vue";
import { FileApi } from "@/helpers/api/file";
import { useStore } from "@/store";

const isFileUploaded = ref<boolean>(false);

const isTextTipShown = ref<boolean>(false);
const fileList = ref<UploadFileInfo[]>([]);

const uId = ref<string>("");

const store = useStore();

async function customRequest({ file, onFinish, onError }: UploadCustomRequestOptions) {
  if (file.file === undefined || file.file === null) {
    onError();
  } else {
    try {
      const fileBase64 = await toBase64(file.file);
      await store.dispatch("user/refreshExpired");
      const { data } = await FileApi.create(
        { fileBase64: fileBase64, type: file.file.type },
        store.getters["user/accessToken"]
      );
      uId.value = data.item.id;
      onFinish();
    } catch (error) {
      onError();
    }
  }
}

const message = useMessage();

const onBeforeUpload: OnBeforeUpload = async ({ file }) => {
  if (file.file === null || file.file === undefined) return false;
  if (file.file.size === undefined || file.file.size > 10 * 1024 * 1024) {
    message.error("Размер файла не должен превышать 10 Мб.");
    return false;
  }
  return true;
};

const onFinish: OnFinish = ({ file }) => {
  isFileUploaded.value = true;
  return file;
};

async function downloadFile(): Promise<void> {
  try {
    await store.dispatch("user/refreshExpired");
    const { data } = await FileApi.getLink({ uId: uId.value }, store.getters["user/accessToken"]);
    await downloadUsingFetch(data.item.base64String, "test");
  } catch (error) {
    if (error instanceof Error) {
      console.log(error);
    }
  }
}

async function downloadUsingFetch(dataUrl: string, filename: string) {
  const res = await fetch(dataUrl);
  const fileBlob = await res.blob();
  const fileUrl = URL.createObjectURL(fileBlob);
  const anchor = document.createElement("a");
  anchor.href = fileUrl;
  anchor.download = filename;
  anchor.click();
  URL.revokeObjectURL(fileUrl);
}

</script>

<style scoped lang="scss">
@import "@/components/table/style.scss";
</style>

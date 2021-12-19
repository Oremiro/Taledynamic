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
      <n-a href="/">{{ fileList[0].name }}</n-a>
      <dynamically-typed-button type="error" text>
        <n-icon size="1rem">
          <dismiss-icon />
        </n-icon>
      </dynamically-typed-button>
    </div>
    <n-upload
      v-else
      v-model:file-list="fileList"
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
import { NUpload, NUploadDragger, UploadCustomRequestOptions, UploadFileInfo } from "naive-ui";
import { toBase64 } from "@/helpers";
import { OnBeforeUpload, OnFinish } from "@/components/table/upload";
import { DismissIcon } from "@/components/icons";
import DynamicallyTypedButton from "@/components/DynamicallyTypedButton.vue";

const isFileUploaded = ref<boolean>(false);
const fileBase64 = ref<string | null>(null);

const isTextTipShown = ref<boolean>(false);
const fileList = ref<UploadFileInfo[]>([]);

async function customRequest({ file, onFinish, onError }: UploadCustomRequestOptions) {
  if (file.file === undefined || file.file === null) {
    onError();
  } else {
    try {
      fileBase64.value = await toBase64(file.file);
      onFinish();
    } catch (error) {
      onError();
    }
  }
}

const onBeforeUpload: OnBeforeUpload = async ({ file }) => {
  console.log(file);
  return true;
};

const onFinish: OnFinish = ({ file }) => {
  console.log(file);
  isFileUploaded.value = true;
  return file;
};
</script>

<style scoped lang="scss">
@import "@/components/table/style.scss";
</style>

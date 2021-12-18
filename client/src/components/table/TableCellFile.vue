<template>
  <n-upload
      :max="1"
      :custom-request="customRequest"
      style="height: 100%; position: relative"
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
</template>

<script setup lang="ts">
import { ref } from "vue";
import { NUpload, NUploadDragger, UploadCustomRequestOptions } from "naive-ui";
import { toBase64 } from "@/helpers";
import { OnBeforeUpload, OnFinish } from "@/components/table/upload";

const isFileUploaded = ref<boolean>(false);
const fileBase64 = ref<string | null>(null);

const isTextTipShown = ref<boolean>(false);


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
}

const onFinish: OnFinish = ({ file }) => {
  console.log(file);
  return file;
}

</script>

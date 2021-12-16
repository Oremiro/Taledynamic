<template>
  <n-image v-if="false" width="25" />
  <n-upload
    v-else
    accept="image/png, image/jpeg"
    :max="1"
    list-type="image-card"
    :custom-request="customRequest"
    @before-upload="onBeforeUpload"
  >
    <n-upload-dragger></n-upload-dragger>
  </n-upload>
</template>

<script setup lang="ts">
import { NImage, NUpload, NUploadDragger, UploadCustomRequestOptions, UploadFileInfo } from "naive-ui";

interface OnBeforeUploadOptions {
  file: UploadFileInfo;
  fileList: UploadFileInfo[];
}

async function onBeforeUpload({ file }: OnBeforeUploadOptions) {
  if (file.file?.type !== "image/png" && file.file?.type !== "image/jpeg") {
    return false;
  }
  return true;
}

async function toBase64(file: File) {
  return new Promise((resolve, reject) => {
    const fileReader = new FileReader();
    fileReader.readAsDataURL(file);
    fileReader.onload = () => resolve(fileReader.result);
    fileReader.onerror = (error) => reject(error);
  });
}

async function customRequest({ file, onFinish, onError }: UploadCustomRequestOptions) {
  if (file.file === undefined || file.file === null) {
    onError();
  } else {
    try {
      const result = await toBase64(file.file);
      console.log(result);
      onFinish();
    } catch (error) {
      onError();
    }
  }
}
</script>

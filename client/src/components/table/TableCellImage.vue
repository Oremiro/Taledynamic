<template>
  <div style="height: 34px">
    <div v-if="isFileUploaded" style="display: flex; gap: .6rem;">
      <n-image :height="34" object-fit="contain" :src="fileBase64" />
      <dynamically-typed-button type="error" text @click="deleteImage">
        <n-icon size="1rem">
          <dismiss-icon />
        </n-icon>
      </dynamically-typed-button>
    </div>
    <n-upload
      v-else
      accept="image/png, image/jpeg"
      :max="1"
      :custom-request="customRequest"
      file-list-style="margin: 0"
      style="height: 100%; position: relative"
      @before-upload="onBeforeUpload"
    >
      <n-upload-dragger
        style="
          position: absolute;
          top: 0;
          padding: 0;
          height: 100%;
          display: flex;
          align-items: center;
          justify-content: center;
        "
      >
        <n-text :depth="3"> Нажмите или перетащите </n-text>
      </n-upload-dragger>
    </n-upload>
  </div>
</template>

<script setup lang="ts">
import { ref } from "vue";
import { NImage, NUpload, NUploadDragger, UploadCustomRequestOptions, UploadFileInfo, useMessage } from "naive-ui";
import { DismissIcon } from "@/components/icons";
import DynamicallyTypedButton from "@/components/DynamicallyTypedButton.vue";

interface OnBeforeUploadOptions {
  file: UploadFileInfo;
  fileList: UploadFileInfo[];
}

const isFileUploaded = ref<boolean>(false);
const fileBase64 = ref<string>();

const message = useMessage();

async function onBeforeUpload({ file }: OnBeforeUploadOptions) {
  if (file.file?.type !== "image/png" && file.file?.type !== "image/jpeg") {
    message.error("Тип изображения может быть только jpeg или png")
    return false;
  }
  return true;
}

async function toBase64(file: File) {
  return new Promise<string>((resolve, reject) => {
    const fileReader = new FileReader();
    fileReader.readAsDataURL(file);
    fileReader.onload = () => {
      if (typeof fileReader.result === "string") {
        resolve(fileReader.result);
      } else {
        reject(new Error("Result isn't base64 string"));
      }
    };
    fileReader.onerror = (error) => reject(error);
  });
}

async function customRequest({ file, onFinish, onError }: UploadCustomRequestOptions) {
  if (file.file === undefined || file.file === null) {
    onError();
  } else {
    try {
      fileBase64.value = await toBase64(file.file);
      isFileUploaded.value = true;
      onFinish();
    } catch (error) {
      onError();
    }
  }
}

function deleteImage() {
  isFileUploaded.value = false;
  fileBase64.value = "";
}
</script>

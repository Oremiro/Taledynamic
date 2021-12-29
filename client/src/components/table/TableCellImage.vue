<template>
  <div style="height: 34px">
    <div v-if="isFileUploaded" style="display: flex; gap: 0.6rem">
      <n-image :height="34" object-fit="contain" :src="fileBase64 ?? ''" />
      <dynamically-typed-button type="error" text @click="deleteImage">
        <n-icon size="1rem">
          <dismiss-icon />
        </n-icon>
      </dynamically-typed-button>
    </div>
    <n-upload
      v-else-if="!disabled"
      accept="image/png, image/jpeg"
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
        <n-text v-show="isTextTipShown" style="font-size: 0.8rem" :depth="3">Перетащите изображение сюда</n-text>
      </n-upload-dragger>
    </n-upload>
  </div>
</template>

<script setup lang="ts">
import { ref, watch } from "vue";
import { NImage, NUpload, NUploadDragger, UploadCustomRequestOptions, useMessage } from "naive-ui";
import { DismissIcon } from "@/components/icons";
import DynamicallyTypedButton from "@/components/DynamicallyTypedButton.vue";
import { toDataURL } from "@/helpers";
import { OnBeforeUpload, OnFinish } from "@/components/table/upload";

const props = defineProps<{
  value: string | null;
  disabled: boolean;
}>();

const emit = defineEmits<{
  (e: "update", value: string | null): void;
}>();

const isFileUploaded = ref<boolean>(false);
const fileBase64 = ref<string | null>(props.value);
const isTextTipShown = ref<boolean>(false);

watch(
  () => props.value,
  (value) => {
    if (value !== null) isFileUploaded.value = true;
    fileBase64.value = value;
  },
  { immediate: true }
);

const message = useMessage();

const onBeforeUpload: OnBeforeUpload = async ({ file }) => {
  if (file.file?.type !== "image/png" && file.file?.type !== "image/jpeg") {
    message.error("Тип изображения может быть только jpeg или png");
    return false;
  }
  return true;
};

async function customRequest({ file, onFinish, onError }: UploadCustomRequestOptions) {
  if (file.file === undefined || file.file === null) {
    onError();
  } else {
    try {
      fileBase64.value = await toDataURL(file.file);
      onFinish();
    } catch (error) {
      onError();
    }
  }
}

const onFinish: OnFinish = ({ file }) => {
  isFileUploaded.value = true;
  emit("update", fileBase64.value);
  return file;
};

function deleteImage(): void {
  isFileUploaded.value = false;
  fileBase64.value = null;
  emit("update", null);
}
</script>

<style scoped lang="scss">
@import "@/components/table/style.scss";
</style>

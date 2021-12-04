<template>
  <div class="workspaces-list-loading">
    <div class="flex-column-center">
      <transition name="fade" mode="out-in">
        <div v-if="!error" class="flex-column-center">
          <n-spin size="large" />
          <div style="margin-top: 1rem">Рабочие пространства загружаются</div>
        </div>
        <div v-else class="flex-column-center">
          <n-text type="error" tag="div">
            <n-icon size="3.5rem">
              <error-circle-icon />
            </n-icon>
          </n-text>
          <n-text type="error"> Ошибка загрузки пространств </n-text>
          <n-button style="margin-top: 1rem" @click="$emit('repeatLoading')"> Попробовать снова </n-button>
        </div>
      </transition>
    </div>
  </div>
</template>

<script setup lang="ts">
import { NSpin } from "naive-ui";
import ErrorCircleIcon from "@/components/icons/ErrorCircleIcon.vue";

defineEmits<{
  (e: "repeatLoading"): void;
}>();

defineProps({
  error: {
    type: Boolean,
    default: false
  }
});
</script>

<style lang="scss" scoped>
.flex-column-center {
  display: flex;
  flex-direction: column;
  align-items: center;
}
.workspaces-list-loading {
  display: flex;
  align-items: center;
  justify-content: center;
  height: 100%;
}
</style>

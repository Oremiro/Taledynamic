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
              <svg
                xmlns="http://www.w3.org/2000/svg"
                xmlns:xlink="http://www.w3.org/1999/xlink"
                viewBox="0 0 20 20"
              >
                <g fill="none">
                  <path
                    d="M10 2a8 8 0 1 1 0 16a8 8 0 0 1 0-16zm0 1a7 7 0 1 0 0 14a7 7 0 0 0 0-14zm0 9.5a.75.75 0 1 1 0 1.5a.75.75 0 0 1 0-1.5zM10 6a.5.5 0 0 1 .492.41l.008.09V11a.5.5 0 0 1-.992.09L9.5 11V6.5A.5.5 0 0 1 10 6z"
                    fill="currentColor"
                  />
                </g>
              </svg>
            </n-icon>
          </n-text>
          <n-text type="error"> Ошибка загрузки пространств </n-text>
          <n-button style="margin-top: 1rem" @click="$emit('repeatLoading')">
            Попробовать снова
          </n-button>
        </div>
      </transition>
    </div>
  </div>
</template>

<script setup lang="ts">
import { NSpin } from "naive-ui";

defineEmits<{
  (e: "repeatLoading"): void;
}>();

defineProps({
  error: {
    type: Boolean,
    default: false,
  },
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

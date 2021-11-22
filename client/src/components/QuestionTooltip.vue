<template>
  <n-tooltip width="20rem" placement="top" trigger="click">
    <template #trigger>
      <n-button text>
        <n-icon class="question-icon">
          <question-circle-icon />
        </n-icon>
      </n-button>
    </template>
    <span>
      <slot />
    </span>
  </n-tooltip>
</template>

<script setup lang="ts">
import QuestionCircleIcon from "@/components/icons/QuestionCircleIcon.vue";
</script>

<style lang="scss" scoped>
.question-icon:hover {
  color: var(--icon-color-hover);
}
</style>

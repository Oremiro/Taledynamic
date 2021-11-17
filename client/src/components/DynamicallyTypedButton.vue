<template>
  <n-button
    :type="isHovered || isFocused ? type : 'default'"
    @mouseenter="isHovered = true"
    @mouseleave="isHovered = false"
    @focusin="isFocused = true"
    @focusout="isFocused = false"
  >
    <slot />
    <template v-if="$slots.icon" #icon>
      <slot name="icon" />
    </template>
  </n-button>
</template>
<script setup lang="ts">
import { ref, PropType } from "vue";

defineProps({
  type: {
    required: true,
    type: String as PropType<"success" | "info" | "warning" | "error">
  }
});

const isHovered = ref<boolean>(false);
const isFocused = ref<boolean>(false);
</script>

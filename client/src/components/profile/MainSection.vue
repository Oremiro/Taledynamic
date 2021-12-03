<template>
  <n-text type="primary" style="font-size: 1.25rem">
    {{ store.getters["user/email"] }}
  </n-text>
</template>

<script setup lang="ts">
import { useStore } from "@/store";

const store = useStore();
</script>

<style lang="scss" scoped></style>

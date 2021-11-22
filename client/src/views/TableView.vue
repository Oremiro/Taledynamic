<template>
  <div>
    {{ workspaceId }}
    {{ tableId }}
  </div>
</template>

<script setup lang="ts">
defineProps<{
  workspaceId: string;
  tableId: string;
}>();
</script>

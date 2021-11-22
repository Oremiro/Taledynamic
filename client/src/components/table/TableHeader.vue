<template>
  <div style="display: flex; gap: 1rem; align-items: center">
    <n-input v-model:value="headerName" autosize size="small" :style="{ 'background-color': tableHeaderColor }" />
    <n-button quaternary size="tiny">
      <n-icon size="1.1rem">
        <arrow-sort-icon></arrow-sort-icon>
      </n-icon>
    </n-button>
  </div>
</template>

<script setup lang="ts">
import { ref, computed } from "vue";
import { useThemeVars } from "naive-ui";
import { ArrowSortIcon } from "@/components/icons";

const props = defineProps<{
  name: string;
}>();

const headerName = ref<string>(props.name);

const tableHeaderColor = computed<string>(() => useThemeVars().value.tableHeaderColor);
</script>

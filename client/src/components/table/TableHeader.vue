<template>
  <div
    style="
      display: flex;
      align-items: center;
      justify-content: space-between;
      padding: 0.8rem;
    "
  >
    <div style="display: flex; gap: 0.5rem; align-items: center">
      <n-input
        v-model:value="headerName"
        autosize
        size="small"
        :style="{ 'background-color': tableHeaderColor }"
      />
      <n-button quaternary size="tiny">
        <n-icon size="1.1rem">
          <delete-icon />
        </n-icon>
      </n-button>
    </div>
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
import { ArrowSortIcon, DeleteIcon } from "@/components/icons";

const props = defineProps<{
  name: string;
}>();

const headerName = ref<string>(props.name);

const tableHeaderColor = computed<string>(
  () => useThemeVars().value.tableHeaderColor
);
</script>

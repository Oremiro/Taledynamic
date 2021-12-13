<template>
  <n-layout>
    <n-scrollbar x-scrollable style="padding-bottom: 1rem">
      <table-menu :workspace-id="workspaceId" :table-id="tableId" />
      <n-table :single-line="false" style="width: max-content; margin-right: 1rem">
        <table-head-vue />
        <table-body-vue />
      </n-table>
    </n-scrollbar>
  </n-layout>
</template>

<script setup lang="ts">
import { computed, watch } from "vue";
import { onBeforeRouteUpdate } from "vue-router";
import { NTable } from "naive-ui";
import { useStore } from "@/store";
import TableHeadVue from "@/components/table/TableHead.vue";
import TableBodyVue from "@/components/table/TableBody.vue";
import TableMenu from "@/components/table/TableMenu.vue";

const props = defineProps<{
  workspaceId: string;
  tableId: string;
}>();

const workspaceId = computed<number>(() => parseInt(props.workspaceId));
const tableId = computed<number>(() => parseInt(props.tableId));

const store = useStore();

onBeforeRouteUpdate(() => {
  return true;
});

watch(
  () => props.tableId,
  async () => {
    await store.dispatch("table/pullTable", { tableId: tableId.value });
  },
  { immediate: true }
);
</script>

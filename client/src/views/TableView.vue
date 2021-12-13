<template>
  <n-layout embedded>
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
import { onBeforeRouteLeave, onBeforeRouteUpdate } from "vue-router";
import { NTable, useDialog } from "naive-ui";
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

const dialog = useDialog();

async function onBeforeRoute() {
  if (store.getters["table/isUpdated"]) return true;
  return new Promise<boolean>((resolve) => {
    const dialogReactive = dialog.warning({
      title: "Несохраненные изменения",
      content: "В таблице есть несохраненные изменения. Если вы покинете страницу, они будут утеряны.",
      positiveText: "Сохранить и покинуть",
      negativeText: "Покинуть",
      onPositiveClick: async () => {
        try {
          dialogReactive.loading = true;
          await store.dispatch("table/pushTable");
          resolve(true);
        } catch (error) {
          if (error instanceof Error) {
            console.log(error);
          }
        }
      },
      onNegativeClick: () => {
        resolve(true);
      },
      onClose: () => {
        resolve(false);
      }
    });
  });
}

onBeforeRouteLeave(onBeforeRoute);
onBeforeRouteUpdate(onBeforeRoute);

watch(
  () => props.tableId,
  async () => {
    await store.dispatch("table/pullTable", { tableId: tableId.value });
  },
  { immediate: true }
);
</script>

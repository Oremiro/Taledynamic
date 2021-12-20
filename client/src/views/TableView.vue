<template>
  <n-result
    v-if="isLoadingError"
    status="error"
    title="Ошибка при загрузке таблицы"
    description="Возможно, Вы делаете что-то не так"
    style="margin-top: 5rem"
  />
  <n-layout v-else embedded>
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
import { ref, computed, watch } from "vue";
import { onBeforeRouteLeave, onBeforeRouteUpdate } from "vue-router";
import { NTable, useDialog, useMessage } from "naive-ui";
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

const message = useMessage();

const isLoadingError = ref<boolean>(false);

watch(
  () => props.tableId,
  async () => {
    try {
      await store.dispatch("table/pullTable", { tableId: tableId.value });
    } catch (error) {
      isLoadingError.value = true;
      if (error instanceof Error) {
        message.error("При загрузке таблицы возникла ошибка");
      }
    }
  },
  { immediate: true }
);
</script>

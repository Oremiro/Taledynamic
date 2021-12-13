<template>
  <n-layout>
    <n-scrollbar x-scrollable style="padding-bottom: 1rem">
    <n-button @click="pushTable">test</n-button>
      <table-menu :workspace-id="workspaceId" :table-id="tableId" />
      <n-table :single-line="false" style="width: max-content; margin-right: 1rem">
        <table-head-vue />
        <table-body-vue />
      </n-table>
    </n-scrollbar>
  </n-layout>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from "vue";
import { onBeforeRouteUpdate } from "vue-router";
import { NTable } from "naive-ui";
import { useStore } from "@/store";
import TableHeadVue from "@/components/table/TableHead.vue";
import TableBodyVue from "@/components/table/TableBody.vue";
import TableMenu from "@/components/table/TableMenu.vue";
import { TableDataApi } from "@/helpers/api/tableData";

const props = defineProps<{
  workspaceId: string;
  tableId: string;
}>();

const workspaceId = computed<number>(() => parseInt(props.workspaceId));
const tableId = computed<number>(() => parseInt(props.tableId));

const uId = ref<string>();

async function pullTable(): Promise<void> {
  await store.dispatch("user/refreshExpired");
  const { data } = await TableDataApi.get({ id: tableId.value }, store.getters["user/accessToken"]);
  uId.value = data.item.uId;
  await store.dispatch("table/setJsonTable", { jsonTable: data.item.tableData });
}

async function pushTable(): Promise<void> {
  if (uId.value === undefined) return;
  await store.dispatch("user/refreshExpired");
  console.log(uId.value);
  console.log(store.getters["table/tableJson"]);
  const { data } = await TableDataApi.update(
    { uId: uId.value, jsonContent: store.getters["table/tableJson"] },
    store.getters["user/accessToken"]
  );
  console.log(data.statusCode);
}

onMounted(async () => {
  await pullTable();
});

onBeforeRouteUpdate(() => {
  return true;
});

const store = useStore();
</script>

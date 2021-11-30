<template>
  <div class="container">
    <n-table :single-line="false">
      <table-head-vue />
      <table-body-vue />
    </n-table>
  </div>
</template>

<script setup lang="ts">
import { NTable } from "naive-ui";
import {
  TableRow,
  TableHeader,
  TableDataType,
  TableCell
} from "@/models/table";
import TableHeadVue from "@/components/table/TableHead.vue";
import TableBodyVue from "@/components/table/TableBody.vue";
import { useStore } from "@/store";

defineProps<{
  workspaceId: string;
  tableId: string;
}>();

// const workspaceId = computed<number>(() => parseInt(props.workspaceId));
// const tableId = computed<number>(() => parseInt(props.tableId));

const tableHeaders: TableHeader[] = [
  new TableHeader("Товар", TableDataType.Text),
  new TableHeader("Стоимость", TableDataType.Number),
  new TableHeader("Количество", TableDataType.Number),
  new TableHeader("Дата производства", TableDataType.Date)
];
const tableRows: TableRow[] = [
  new TableRow([
    new TableCell("Пылесос", TableDataType.Text),
    new TableCell(20000, TableDataType.Number),
    new TableCell(3, TableDataType.Number),
    new TableCell(new Date(Date.now()), TableDataType.Date)
  ]),
  new TableRow([
    new TableCell("Ноутбук", TableDataType.Text),
    new TableCell(100000, TableDataType.Number),
    new TableCell(1, TableDataType.Number),
    new TableCell(new Date(2021, 1, 10), TableDataType.Date)
  ])
];

const store = useStore();
store.commit("table/setTable", { headers: tableHeaders, rows: tableRows });

</script>

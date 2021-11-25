<template>
  <div class="container" style="padding: 2rem">
    <n-table :single-line="false">
      <table-head-vue
        :data="tableHeaders"
        @swap="swapTableHeadersItems"
        @create="addColumn"
      />
      <table-body-vue
        :data="tableRows"
        :row-length="tableHeaders.length"
        @swap="swapTableBodyItems"
      />
    </n-table>
  </div>
</template>

<script setup lang="ts">
import { reactive, computed } from "vue";
import { NTable } from "naive-ui";
import {
  TableRow,
  TableHeader,
  TableDataType,
  TableCell
} from "@/models/table";
import TableHeadVue from "@/components/table/TableHead.vue";
import TableBodyVue from "@/components/table/TableBody.vue";

const props = defineProps<{
  workspaceId: string;
  tableId: string;
}>();

const workspaceId = computed<number>(() => parseInt(props.workspaceId));
const tableId = computed<number>(() => parseInt(props.tableId));

const tableHeaders = reactive<Array<TableHeader>>([
  new TableHeader("Товар", TableDataType.Text),
  new TableHeader("Стоимость", TableDataType.Number),
  new TableHeader("Количество", TableDataType.Number),
  new TableHeader("Дата производства", TableDataType.Date)
]);
const tableRows = reactive<Array<TableRow>>([
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
    new TableCell(new Date(Date.now()), TableDataType.Date)
  ]),
  new TableRow([
    new TableCell("", TableDataType.Text),
    new TableCell("", TableDataType.Number),
    new TableCell("", TableDataType.Number),
    new TableCell("", TableDataType.Date)
  ])
]);

async function swapTableHeadersItems(
  indexFirst: number,
  indexSecond: number
): Promise<void> {
  [tableHeaders[indexFirst], tableHeaders[indexSecond]] = [
    tableHeaders[indexSecond],
    tableHeaders[indexFirst]
  ];
  for (let tableRow of tableRows) {
    [tableRow.cells[indexFirst], tableRow.cells[indexSecond]] = [
      tableRow.cells[indexSecond],
      tableRow.cells[indexFirst]
    ];
  }
}

async function swapTableBodyItems(
  indexFirst: number,
  indexSecond: number
): Promise<void> {
  [tableRows[indexFirst], tableRows[indexSecond]] = [
    tableRows[indexSecond],
    tableRows[indexFirst]
  ];
}

async function addColumn(name: string) {
  tableHeaders.push(new TableHeader(name, TableDataType.Text));
  for (let tableRow of tableRows) {
    tableRow.cells.push(new TableCell("", TableDataType.Text));
  }
}
</script>

<template>
  <div class="container" style="padding: 2rem">
    <n-table :single-line="false">
      <thead>
        <tr>
          <th v-for="header of tableHeaders" :key="header.id">
            <table-header-vue :name="header.name" />
          </th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="row in tableRows" :key="row.id">
          <td v-for="cell in row.cells" :key="cell.id" style="padding: 0">
            <table-cell-vue :data="cell.data" :type="cell.type" />
          </td>
        </tr>
        <tr>
          <td
            v-for="cell of tableHeaders.length"
            :key="cell"
            style="padding: 0"
          >
            <table-cell-vue :type="0" />
          </td>
        </tr>
      </tbody>
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
import TableCellVue from "@/components/table/TableCell.vue";
import TableHeaderVue from "@/components/table/TableHeader.vue";

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
  ])
]);
</script>

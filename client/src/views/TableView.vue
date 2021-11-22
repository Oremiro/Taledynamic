<template>
  <div class="container" style="padding: 2rem">
    <n-table :single-line="false">
      <thead>
        <tr>
          <th v-for="header of tableHeaders" :key="header.id">
            <div style="display: flex; gap: 1rem; align-items: center">
              <div>{{ header.name }}</div>
              <n-button quaternary size="tiny">
                <n-icon size="1.1rem">
                  <arrow-sort-icon></arrow-sort-icon>
                </n-icon>
              </n-button>
            </div>
          </th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="row in tableRows" :key="row.id">
          <td v-for="cell in row.cells" :key="cell.id" style="padding: 0">
            <n-input
              v-model:value="cell.data"
              :style="{ 'background-color': tableColor, padding: '.2rem 0' }"
              placeholder=""
            />
          </td>
        </tr>
        <tr>
          <td
            v-for="cell of tableHeaders.length"
            :key="cell"
            style="padding: 0"
          >
            <n-input
              :style="{ 'background-color': tableColor, padding: '.2rem 0' }"
              placeholder=""
            />
          </td>
        </tr>
      </tbody>
    </n-table>
  </div>
</template>

<script setup lang="ts">
import { reactive, computed } from "vue";
import { NTable, useThemeVars } from "naive-ui";
import {
  TableRow,
  TableHeader,
  TableDataType,
  TableCell
} from "@/models/table";
import { ArrowSortIcon } from "@/components/icons";

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
  new TableHeader("Сумма", TableDataType.Number)
]);
const tableRows = reactive<Array<TableRow>>([
  new TableRow([
    new TableCell("Пылесос", TableDataType.Text),
    new TableCell(20000, TableDataType.Number),
    new TableCell(3, TableDataType.Number),
    new TableCell(60000, TableDataType.Number)
  ])
]);

const tableColor = computed<string>(() => useThemeVars().value.tableColor);
</script>

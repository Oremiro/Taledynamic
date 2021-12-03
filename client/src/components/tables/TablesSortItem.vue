<template>
  <n-popselect v-model:value="popSortValue" trigger="click" :options="popOptions" @update:value="onUpdateValue">
    <n-button text>
      <n-icon size="1.2rem">
        <arrow-sort-icon />
      </n-icon>
    </n-button>
  </n-popselect>
</template>

<script setup lang="ts">
import { ref } from "vue";
import { SelectGroupOption, SelectOption } from "naive-ui";
import { ArrowSortIcon } from "@/components/icons";
import { TablesSortType } from "@/models";

const emit = defineEmits<{
  (e: "update", value: TablesSortType): void;
}>();

const popOptions: Array<SelectOption | SelectGroupOption> = [
  {
    type: "group",
    label: "По названию",
    key: "sortByName",
    children: [
      {
        label: "От A до Z",
        value: TablesSortType.NameAscending
      },
      {
        label: "От Z до A",
        value: TablesSortType.NameDescending
      }
    ]
  }
];

const popSortValueStored: string | null = localStorage.getItem("tablesSort");
const popSortValueParsed: number = popSortValueStored ? parseInt(popSortValueStored) : TablesSortType.NameAscending;
const popSortValue = ref<TablesSortType>(
  !isNaN(popSortValueParsed) ? popSortValueParsed : TablesSortType.NameDescending
);
emit("update", popSortValue.value);

function onUpdateValue(value: TablesSortType) {
  emit("update", value);
}
</script>

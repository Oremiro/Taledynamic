<template>
  <n-card>
    <template #header>
      <n-page-header v-if="isInitializationSuccess">
        <template #title>
          <n-text type="success">{{ currentWorkspace?.name }}</n-text>
        </template>
        <template #subtitle>
          <div>Дата изменения: {{ currentWorkspace?.modified.toLocaleString() }}</div>
        </template>
        <template #extra>
          <div v-if="tablesListRef?.tables.length !== 0" style="display: flex; gap: 0.5rem">
            <n-button text @click="isTablesEditable = !isTablesEditable">
              <n-icon size="1.2rem">
                <edit-icon />
              </n-icon>
            </n-button>
            <n-popselect
              v-model:value="popSortValue"
              trigger="click"
              :options="popOptions"
              @update:value="updateHandler"
            >
              <n-button text>
                <n-icon size="1.2rem">
                  <arrow-sort-icon />
                </n-icon>
              </n-button>
            </n-popselect>
          </div>
        </template>
      </n-page-header>
      <n-skeleton v-else text round />
      <n-divider style="margin-bottom: 0" />
    </template>
    <template #default>
      <tables-list
        ref="tablesListRef"
        :workspace-id="workspaceId"
        :editable="isTablesEditable"
        :sort-type="popSortValue"
      />
    </template>
  </n-card>
</template>

<script setup lang="ts">
import { computed, ref } from "vue";
import { NPageHeader, NDivider, NSkeleton, SelectOption, SelectGroupOption } from "naive-ui";
import { useStore } from "@/store";
import { Workspace } from "@/models/store";
import { InitializationStatus, TablesSortType } from "@/models";
import TablesList from "@/components/tables/TablesList.vue";
import { ArrowSortIcon, EditIcon } from "@/components/icons";

const store = useStore();
const currentWorkspace = computed<Workspace | null>(() => store.getters["workspaces/currentWorkspace"]);

const isTablesEditable = ref<boolean>(false);

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

const tablesListRef = ref<InstanceType<typeof TablesList>>();

async function updateHandler(value: number): Promise<void> {
  localStorage.setItem("tablesSort", value.toString());
  // @ts-expect-error: vue-next #4397
  await tablesListRef.value?.sortList(popSortValue.value);
}

const isInitializationSuccess = computed<boolean>(() => workspacesInitStatus.value === InitializationStatus.Success);

/* Setting current workspace */
const workspacesInitStatus = computed<InitializationStatus>(() => store.getters["workspaces/initStatus"]);
</script>

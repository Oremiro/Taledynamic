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
            <tables-sort-item @update="onSortTypeUpdate" />
          </div>
        </template>
      </n-page-header>
      <n-skeleton v-else text round />
      <n-divider style="margin-bottom: 0" />
    </template>
    <template #default>
      <tables-list
        v-if="currentWorkspace !== null"
        ref="tablesListRef"
        :workspace-id="currentWorkspace.id"
        :editable="isTablesEditable"
        :sort-type="sortValue"
      />
    </template>
  </n-card>
</template>

<script setup lang="ts">
import { computed, ref } from "vue";
import { NPageHeader, NDivider, NSkeleton } from "naive-ui";
import { useStore } from "@/store";
import { Workspace } from "@/models/store";
import { InitializationStatus, TablesSortType } from "@/models";
import TablesList from "@/components/tables/TablesList.vue";
import { EditIcon } from "@/components/icons";
import TablesSortItem from "@/components/tables/TablesSortItem.vue";

const store = useStore();
const currentWorkspace = computed<Workspace | null>(() => store.getters["workspaces/currentWorkspace"]);

const isTablesEditable = ref<boolean>(false);




const tablesListRef = ref<InstanceType<typeof TablesList>>();

const sortValue = ref<TablesSortType>();
async function onSortTypeUpdate(value: TablesSortType): Promise<void> {
  localStorage.setItem("tablesSort", value.toString());
  // @ts-expect-error: vue-next #4397
  await tablesListRef.value?.sortList(value);
  sortValue.value = value;
}

const isInitializationSuccess = computed<boolean>(() => workspacesInitStatus.value === InitializationStatus.Success);

/* Setting current workspace */
const workspacesInitStatus = computed<InitializationStatus>(() => store.getters["workspaces/initStatus"]);
</script>

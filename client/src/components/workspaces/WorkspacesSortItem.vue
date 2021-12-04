<template>
  <n-popselect v-model:value="popSortValue" trigger="click" :options="sortOptions" @update:value="onUpdateSortValue">
    <n-button text>
      <n-icon size="1.2rem">
        <arrow-sort-icon />
      </n-icon>
    </n-button>
  </n-popselect>
</template>

<script setup lang="ts">
import { ref, watch } from "vue";
import { SelectGroupOption, SelectOption } from "naive-ui";
import { ArrowSortIcon } from "@/components/icons";
import { Workspace, WorkspacesSortType } from "@/models/store";
import { useStore } from "@/store";
import { InitializationStatus } from "@/models";

const sortOptions: Array<SelectOption | SelectGroupOption> = [
  {
    type: "group",
    label: "По дате",
    key: "sortByDate",
    children: [
      {
        label: "Сначала новые",
        value: WorkspacesSortType.DateDescending
      },
      {
        label: "Сначала старые",
        value: WorkspacesSortType.DateAscending
      }
    ]
  },
  {
    type: "group",
    label: "По названию",
    key: "sortByName",
    children: [
      {
        label: "От A до Z",
        value: WorkspacesSortType.NameAscending
      },
      {
        label: "От Z до A",
        value: WorkspacesSortType.NameDescending
      }
    ]
  }
];

const sortValueStored: string | null = localStorage.getItem("workspacesSort");
const sortValueParsed: number = sortValueStored ? parseInt(sortValueStored) : WorkspacesSortType.DateDescending;
const popSortValue = ref<WorkspacesSortType>(
  !isNaN(sortValueParsed) ? sortValueParsed : WorkspacesSortType.DateDescending
);

const store = useStore();

async function onUpdateSortValue(value: WorkspacesSortType): Promise<void> {
  localStorage.setItem("workspacesSort", value.toString());
  await store.dispatch("workspaces/sort", { sortType: value });
}

watch(
  () => store.getters["workspaces/initStatus"],
  async (value: InitializationStatus) => {
    if (value === InitializationStatus.Success) {
      await store.dispatch("workspaces/sort", { sortType: popSortValue.value });
      if (store.getters["workspaces/workspaces"].length > 0) {
        const firstWorkspace: Workspace = store.getters["workspaces/workspaces"][0];
        store.commit("workspaces/setCurrentId", { workspaceId: firstWorkspace.id });
      }
    }
  },
  { immediate: true }
);
</script>

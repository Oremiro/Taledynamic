<template>
  <n-menu :value="currentWorkspaceId" :options="menuOptions" :indent="22" @update:value="onUpdateValue" />
</template>

<script setup lang="ts">
import { computed, h } from "vue";
import { MenuGroupOption, MenuOption } from "naive-ui";
import { Workspace } from "@/models/store";
import WorkspacesListItem from "@/components/workspaces/WorkspacesListItem.vue";
import { useStore } from "@/store";

const store = useStore();
const workspaces = computed<Workspace[]>(() => store.getters["workspaces/workspaces"]);
const currentWorkspaceId = computed<number | null>(() => store.getters["workspaces/currentWorkspaceId"]);

const menuOptions = computed<Array<MenuOption | MenuGroupOption>>(() =>
  workspaces.value.map((item: Workspace) => {
    return {
      label: () => h(WorkspacesListItem, { id: item.id, name: item.name }),
      key: item.id
    };
  })
);

async function onUpdateValue(id: number): Promise<void> {
  const workspace: Workspace | undefined = workspaces.value.find((item) => item.id === id);
  if (workspace) {
    store.commit("workspaces/setCurrentId", { workspaceId: workspace.id });
  }
}
</script>

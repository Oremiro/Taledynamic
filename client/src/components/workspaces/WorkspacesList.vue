<template>
  {{ currentWorkspaceId }}
  <n-menu
    :value="currentWorkspaceId"
    :options="menuOptions"
    :indent="22"
    style="padding: 0 0.25rem"
    @update:value="onUpdateValue"
  />
</template>

<script setup lang="ts">
import { computed, h } from "vue";
import { MenuGroupOption, MenuOption } from "naive-ui";
import { Workspace } from "@/models/store";
import WorkspacesListItem from "@/components/workspaces/WorkspacesListItem.vue";
import { useStore } from "@/store";
import { useRouter } from "vue-router";

const store = useStore();
const workspaces = computed<Workspace[]>(() => store.getters["workspaces/workspaces"]);
const currentWorkspaceId = computed<number>(() => store.getters["workspaces/currentWorkspace"]?.id);

const menuOptions = computed<Array<MenuOption | MenuGroupOption>>(() =>
  workspaces.value.map((item: Workspace) => {
    return {
      label: () => h(WorkspacesListItem, { id: item.id, name: item.name }),
      key: item.id
    };
  })
);

const router = useRouter();
async function setCurrentWorkspace(id: number): Promise<void> {
  const currentWorkspace: Workspace | undefined = workspaces.value.find((item) => item.id === id);
  if (currentWorkspace) {
    store.commit("workspaces/setCurrent", { workspace: currentWorkspace });
  } else {
    router.push({ name: "NotFound" });
  }
}

function onUpdateValue(key: number) {
  setCurrentWorkspace(key);
}
</script>

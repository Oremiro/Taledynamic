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
import { computed, h, PropType } from "vue";
import { MenuGroupOption, MenuOption } from "naive-ui";
import { Workspace } from "@/models/store";
import WorkspacesListItem from "@/components/workspaces/WorkspacesListItem.vue";
import { useStore } from "@/store";
import { useRouter } from "vue-router";

const props = defineProps({
  workspaces: {
    type: Array as PropType<Workspace[]>,
    required: true
  }
});

const store = useStore();
const currentWorkspaceId = computed<number>(() => store.getters["workspaces/currentWorkspace"]?.id);

const menuOptions = computed<Array<MenuOption | MenuGroupOption>>(() =>
  props.workspaces.map((item: Workspace) => {
    return {
      label: () => h(WorkspacesListItem, { id: item.id, name: item.name }),
      key: item.id
    };
  })
);

const router = useRouter();
async function setCurrentWorkspace(id: number): Promise<void> {
  if (isNaN(id)) {
    router.push({ name: "NotFound" });
    return;
  }
  const currentWorkspace: Workspace | undefined = props.workspaces.find((item) => item.id === id);
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

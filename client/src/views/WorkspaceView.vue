<template>
  <div class="container">
    <n-card style="max-width: 40rem" >
      <n-page-header>
        <template #title>
          <div>{{ currentWorkspace?.name }}</div>
        </template>
        <template #subtitle>
          <div>{{ currentWorkspace?.created }}</div>
        </template>
      </n-page-header>
      <table-creating-item />
    </n-card>
  </div>
</template>

<script setup lang="ts">
import { computed, watch } from "vue";
import { NPageHeader } from "naive-ui";
import { useRouter } from "vue-router";
import { useStore } from "@/store";
import { Workspace, WorkspacesInitStatus } from "@/interfaces/store";
import TableCreatingItem from "@/components/tables/TableCreatingItem.vue";

const props = defineProps({
  id: {
    type: String,
    required: true
  }
});

const workspaceId = computed<number>(() => parseInt(props.id));

const store = useStore();
const workspaces = computed<Workspace[]>(() => store.getters["workspaces/workspaces"]);
const currentWorkspace = computed<Workspace | null>(() => store.getters["workspaces/currentWorkspace"]);

const router = useRouter();

async function setCurrentWorkspace(id: number): Promise<void> {
  if (isNaN(id)) {
    router.push({ name: "NotFound" });
    return;
  }
  const currentWorkspace: Workspace | undefined = workspaces.value.find(
    (item) => item.id === id
  );
  if (currentWorkspace) {
    store.commit("workspaces/setCurrent", { workspace: currentWorkspace });
  } else {
    router.push({ name: "NotFound" });
  }
}

const workspacesInitStatus = computed<WorkspacesInitStatus>(() => store.getters["workspaces/initStatus"]);
watch(workspacesInitStatus, (value) => {
  if (value === WorkspacesInitStatus.Success) {
    setCurrentWorkspace(workspaceId.value);
  }
})
watch(workspaceId, (value) => {
  setCurrentWorkspace(value);
});
</script>

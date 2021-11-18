<template>
  <div>
    {{ store.getters["workspaces/currentWorkspace"] }}
  </div>
</template>

<script setup lang="ts">
import { computed, watch } from "vue";
import { Workspace } from "@/interfaces/store";
import { useStore } from "@/store";
import { useMessage } from "naive-ui";
import { useRouter } from "vue-router";

const props = defineProps({
  id: {
    type: String,
    required: true
  }
});

const workspaceId = computed<number>(() => parseInt(props.id));

const store = useStore();
const message = useMessage();
async function getWorkspaces(): Promise<void> {
  try {
    await store.dispatch("workspaces/init");
  } catch (error) {
    if (error instanceof Error) {
      message.error(error.message);
    }
  }
}

const router = useRouter();

async function setCurrentWorkspace(id: number): Promise<void> {
  if (isNaN(id)) {
    router.push({ name: "NotFound" });
    return;
  }
  let workspaces: Workspace[] = store.getters["workspaces/workspaces"];
  if (!workspaces.length) {
    await getWorkspaces();
    workspaces = store.getters["workspaces/workspaces"];
  }
  const currentWorkspace: Workspace | undefined = workspaces.find(
    (item) => item.id === id
  );
  if (currentWorkspace) {
    store.commit("workspaces/setCurrent", { workspace: currentWorkspace });
  } else {
    router.push({ name: "NotFound" });
  }
}

setCurrentWorkspace(workspaceId.value);
watch(workspaceId, (value) => {
  setCurrentWorkspace(value);
});
</script>

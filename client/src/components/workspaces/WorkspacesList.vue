<template>
  <n-menu
    :value="$route.path"
    :options="menuOptions"
    :indent="22"
    style="padding: 0 0.25rem"
  />
</template>

<script setup lang="ts">
import { computed, h, PropType } from "vue";
import { MenuGroupOption, MenuOption } from "naive-ui";
import { Workspace } from "@/models/store";
import WorkspacesListItem from "@/components/workspaces/WorkspacesListItem.vue";

const props = defineProps({
  workspaces: {
    type: Array as PropType<Workspace[]>,
    required: true
  }
});

const menuOptions = computed<Array<MenuOption | MenuGroupOption>>(() =>
  props.workspaces.map((item: Workspace) => {
    return {
      label: () => h(WorkspacesListItem, { id: item.id, name: item.name }),
      key: `/workspace/${item.id}`
    };
  })
);
</script>

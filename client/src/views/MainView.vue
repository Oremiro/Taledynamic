<template>
  <div class="container">
    <n-grid :cols="12" :x-gap="20">
      <n-gi :span="3">
        <workspaces-section />
      </n-gi>
      <n-gi :span="9">
        <transition name="fade" mode="out-in">
          <tables-section v-if="currentWorkspaceId" />
          <n-card
            v-else
            style="height: 100%"
            content-style="display: flex; align-items: center; justify-content: center"
          >
            <n-empty size="large">
              <template #default>
                <div style="text-align: center;">
                  Здесь будет отображаться список ваших таблиц.<br>
                  Для начала создайте рабочее пространство.
                </div>
              </template>
              <template #icon>
                <n-icon>
                  <apps-list-icon />
                </n-icon>
              </template>
            </n-empty>
          </n-card>
        </transition>
      </n-gi>
    </n-grid>
  </div>
</template>

<script setup lang="ts">
import { computed } from "vue";
import { useStore } from "@/store";
import WorkspacesSection from "@/components/workspaces/WorkspacesSection.vue";
import TablesSection from "@/components/tables/TablesSection.vue";
import { AppsListIcon } from "@/components/icons";

const store = useStore();
const currentWorkspaceId = computed<number | null>(() => store.getters["workspaces/currentWorkspaceId"]);
</script>

<style lang="scss" scoped></style>

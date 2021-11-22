import { createApp } from "vue";
import { VueCookieNext } from "vue-cookie-next";
import App from "./App.vue";
import router from "./router";
import { store, key } from "./store";
import {
  create,
  NButton,
  NConfigProvider,
  NGlobalStyle,
  NLayoutHeader,
  NMenu,
  NText,
  NSpace,
  NForm,
  NFormItem,
  NInput,
  NInputGroup,
  NCheckbox,
  NCard,
  NTabs,
  NTabPane,
  NH1,
  NH2,
  NH3,
  NH4,
  NA,
  NGrid,
  NGridItem,
  NIcon,
  NLayout,
  NLayoutContent,
  NResult,
  NMessageProvider,
  NAutoComplete,
  NTooltip,
  NCollapseTransition,
  NButtonGroup,
  NLoadingBarProvider,
  NLayoutSider,
  NPopconfirm
} from "naive-ui";

const naive = create({
  components: [
    NButton,
    NConfigProvider,
    NGlobalStyle,
    NLayoutHeader,
    NMenu,
    NText,
    NSpace,
    NForm,
    NFormItem,
    NInput,
    NInputGroup,
    NCheckbox,
    NCard,
    NTabs,
    NTabPane,
    NH1,
    NH2,
    NH3,
    NH4,
    NA,
    NGrid,
    NGridItem,
    NIcon,
    NLayout,
    NLayoutContent,
    NResult,
    NMessageProvider,
    NAutoComplete,
    NTooltip,
    NCollapseTransition,
    NButtonGroup,
    NLoadingBarProvider,
    NLayoutSider,
    NPopconfirm
  ]
});

createApp(App)
  .use(store, key)
  .use(router)
  .use(naive)
  .use(VueCookieNext)
  .mount("#app");

import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import { 
    create, 
    NButton, NConfigProvider, NGlobalStyle, 
    NLayoutHeader, NMenu, NText, NSpace,
    NForm, NFormItem, NInput, NCheckbox,
    NCard, NTabs, NTabPane, NH1, NA, NGrid, NGridItem,
    NIcon, NLayout, NLayoutContent, NResult
} from 'naive-ui'

const naive = create({
    components: [
        NButton, NConfigProvider, NGlobalStyle, 
        NLayoutHeader, NMenu, NText, NSpace, 
        NForm, NFormItem, NInput, NCheckbox,
        NCard, NTabs, NTabPane, NH1, NA, NGrid,
        NGridItem, NIcon, NLayout, NLayoutContent,
        NResult
    ]
})

createApp(App).use(router).use(naive).mount('#app')

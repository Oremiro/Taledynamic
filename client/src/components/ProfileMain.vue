<template>
  <div class="card-header">
    <div class="user-img-background">
        <img class="user-img" src="https://picsum.photos/500" alt="">
    </div>
    <div class="user-info-background">
        <n-text type="primary" style="font-size: 1.25rem">1asagne@mail.ru</n-text>
        <div>Активный пользователь</div>
    </div>
    </div>
    <div>Какие-то основные данные. <br> Какая-то статистика. <br> Lorem ipsum dolor sit amet consectetur adipisicing elit. Voluptatum eligendi deleniti similique? Eligendi distinctio fuga eum adipisci nam? Optio porro doloremque, odit fuga omnis quas id sed neque qui deleniti et possimus amet commodi nulla numquam eligendi. Officiis quod ab illum ducimus accusamus reiciendis impedit repellendus quasi aliquam? Animi minima saepe laborum dolore id, itaque expedita a dolorem iste earum. Eaque sed error esse perspiciatis id, dolores iure.</div>
</template>

<style lang="scss" scoped>
.card-header {
    display: flex;
    justify-content: space-between;
    padding-bottom: var(--padding-bottom)
}
.user-img {
    max-width: 10rem;
    max-height: 10rem;
    border-radius: var(--border-radius);
}
.user-img-background {
    display: flex;
    justify-content: center;
}
.user-info-background {
    display: flex;
    width: 100%; 
    flex-direction: column;
    padding-left: 2rem;
}
</style>

<script lang="ts">
import { defineComponent } from 'vue'

export default defineComponent({
    name: 'MainProfile'
})
</script>

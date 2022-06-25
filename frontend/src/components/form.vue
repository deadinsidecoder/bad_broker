<template>
    <from class="form" @submit.prevent="submit">
        <n-input-number :value="money" :validator="moneyValidator" :on-update:value="setMoney" >
            <template #prefix>
                $
            </template>
        </n-input-number>
        <n-date-picker type="daterange" 
            :value="range"
            updateValueOnClose
            @confirm="setRange"
        />
        <n-button type="primary" attrType="submit" @click="submit">
            <n-icon>
                <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" viewBox="0 0 16 16"><g fill="none"><path d="M1.724 1.053a.5.5 0 0 0-.714.545l1.403 4.85a.5.5 0 0 0 .397.354l5.69.953c.268.053.268.437 0 .49l-5.69.953a.5.5 0 0 0-.397.354l-1.403 4.85a.5.5 0 0 0 .714.545l13-6.5a.5.5 0 0 0 0-.894l-13-6.5z" fill="currentColor"></path></g></svg>
            </n-icon>
        </n-button>
    </from>
</template>

<script>
import { NDatePicker, NButton, NInputNumber, NIcon } from 'naive-ui'

export default {
    setup(){
        return {
            moneyValidator: (x) => x > 0,
        }
    },

    data(){
        const startDate = new Date().setDate(new Date().getDate() - 7); 
        const endDate = new Date();

        return {
            range: [startDate, endDate],
            money: 100,
        }
    },

    methods: {
        setRange(range){
            this.range = range;
        },

        setMoney(value){
            this.money = value;
        },

        submit(){
            this.$store.dispatch('fetchBestStrategyAndRates', {
                startDate: new Date(this.range[0]),
                endDate: new Date(this.range[1]),
                money: this.money
            });
        }
    },

    components: {
        NDatePicker,
        NButton,
        NInputNumber,
        NIcon
    }
}
</script>

<style lang="scss">
.form{
    display: flex;
    justify-content: center;;
}
</style>
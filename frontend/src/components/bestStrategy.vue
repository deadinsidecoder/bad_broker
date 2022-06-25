<template>
    <div v-show="this.$store.state.bestStrategy.tool" class="best-strategy">
        <span>
            Buy <b>{{tool}}</b> {{buyDate}}, sell {{sellDate}}. 
            Revenue: <b :class="revenueWrapperClass">{{revenuePrefix}}{{revenue}}</b>
        </span>
    </div>
</template>

<script>
export default {
    computed: {
        bestStrategy(){
            return this.$store.state.bestStrategy;  
        },

        tool(){
            return this.bestStrategy.tool?.toUpperCase();
        
        },

        buyDate(){
            return this.bestStrategy.buyDate?.toISOString().split('T')[0]
        },

        sellDate(){
            return this.bestStrategy.sellDate?.toISOString().split('T')[0]
        },

        revenue(){
            return Math.round(this.bestStrategy.revenue * 100) / 100;
        },

        revenuePrefix(){
            return this.revenue < 0 ? '-' : '+';
        },

        revenueWrapperClass(){
            return this.revenue <= 0 ? 'red' : 'green';
        }
    }
}
</script>

<style scoped>
.best-strategy{
    font-size: 16px;
}

.red{
    color: #e88080;
}

.green{
    color:#63e2b7;
}
</style>
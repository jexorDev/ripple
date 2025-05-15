<script setup lang="ts">
//import HelloWorld from './components/HelloWorld.vue'

let timerId: number | null = null;
const results = ref<string[]>([]);

function searchUpdate(searchString: string): void {
    if (timerId) {
      clearTimeout(timerId);
    }

    if (!searchString) {
      transactions.value = [];
      noResults.value = false;
      return;
    }

    timerId = setTimeout(async () => {

      try {
        isLoading.value = true;
        transactions.value = await new TransactionsService()
        .withUrlParameters({
          searchString: searchString
        }).getMultiple();
        
      } finally {
        noResults.value = transactions.value.length === 0;
        isLoading.value = false;
      }

      }, 1000); 
      
  }
</script>

<template>
  <div>
   <v-autocomplete @update:search="searchUpdate"></v-autocomplete>
  </div>
  <!-- <HelloWorld msg="Vite + Vue" /> -->
</template>



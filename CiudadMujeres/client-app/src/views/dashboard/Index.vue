<template>
  <v-app>
    <dashboard-core-app-bar />

    <dashboard-core-drawer />

    <dashboard-core-view />
  </v-app>
</template>

<script>
  export default {
    name: 'DashboardIndex',

    components: {
      DashboardCoreAppBar: () => import('./components/core/AppBar'),
      DashboardCoreDrawer: () => import('./components/core/Drawer'),
      DashboardCoreView: () => import('./components/core/View'),
    },

    data: () => ({
      expandOnHover: false,
    }),

    mounted () {
      // this.$http.post('/api/account/login', { username: 'test@test.com', password: 'admin' })
      //   .then(response => {
      //     console.log(response)
      //     this.$http.get('/api/account/ping')
      //       .then(resp => { console.log(resp) })
      //   })
      //    .then(resp => {
      //  .then(response => {
      //  this.$http.get('/api/account/ping')
      //    .then(resp => {
      //      this.$http.get('/api/account/logout')
      //        .then(res => { console.log(res) })
      //    })
      //  })
    },
  }
</script>

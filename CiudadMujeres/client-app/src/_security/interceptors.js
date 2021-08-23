

this.$http.interceptors.push((request, next) => {
    next((response) => {
        if (response.status === 401) {
            return this.$http.get('api/security/account/credentials/refresh').then((result) => {
                localStorage.setItem('auth-user', JSON.stringify(response.data))

                return this.$http(request).then((response) => {
                    return response
                })
            })
            .catch(() => {
                return this.$router.push({ name: 'Login' }).catch(() => {})
            })
        }
    })
})

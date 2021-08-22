/**
 * This code block can be directly in your main.js file if you keep it this simple
 * otherwise extract it to its own file and require it in main.js
 *
 * Don't forget that the code below will be executed within every request.
 *
 */

this.$http.interceptors.push((request, next) => {
    /**
     * Here is where we can refresh the token.
     */
    next((response) => {
        /**
         * If we get a 401 response from the API means that we are Unauthorized to
         * access the requested end point.
         * This means that probably our token has expired and we need to get a new one.
         */
        if (response.status === 401) {
            return this.$http.get('api/security/account/credentials/refresh').then((result) => {
                // Save the new token on local storage
                localStorage.setItem('auth-user', JSON.stringify(response.data))

                // Resend the failed request whatever it was (GET, POST, PATCH) and return its resposne
                return this.$http(request).then((response) => {
                    return response
                })
            })
            .catch(() => {
                /**
                 * We weren't able to refresh the token so the best thing to do is
                 * logout the user (removing any user information from storage)
                 * and redirecting to login page
                 */
                return this.$router.push({ name: 'Login' }).catch(() => {})
            })
        }
    })
})

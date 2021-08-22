export default function RedirectIfAuthenticated (router) {
    /**
     * If the user is already authenticated he shouldn't be able to visit
     * pages like login, register, etc...
     */
    router.beforeEach((to, from, next, abort, redirect) => {
        const user = window.localStorage.getItem('token')
        /**
         * Checks if there's a token and the next page name is none of the following
         */
        if ((user) && (to.name === 'Login' || to.name === 'register')) {
            // redirects according user role
            router.push({ name: 'Login' })
        }

        if (!user) {
            // Logout
        }

        next()
    })
}

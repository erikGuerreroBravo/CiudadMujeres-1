export default function RedirectIfAuthenticated (router) {
    router.beforeEach((to, from, next, abort, redirect) => {
        const user = window.localStorage.getItem('token')
        if ((user) && (to.name === 'Login' || to.name === 'register')) {
            router.push({ name: 'Login' })
        }

        if (!user) {
            // Logout
        }

        next()
    })
}

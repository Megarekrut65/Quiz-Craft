import os

import cherrypy
import django
from django.conf import settings
from django.core.handlers.wsgi import WSGIHandler
from django.core.management import call_command

import cheroot.ssl  # for pyinstaller
import cheroot.ssl.pyopenssl

import ssl_manager as ssl_m


def read_server_confing():
    try:
        file = open("system/server.conf")
        ip = file.readline().split("=")[1].strip()
        port = file.readline().split("=")[1].strip()
        file.close()

        return ip, int(port)
    except Exception as e:
        print(e)

    return "127.0.0.1", 8000


def mount_static(url, root):
    config = {
        'tools.staticdir.on': True,
        'tools.staticdir.dir': root,
        'tools.expires.on': True,
        'tools.expires.secs': 86400
    }
    cherrypy.tree.mount(None, url, {'/': config})


def cherrypy_configs(ip, port, cert_path, key_path):
    return {
        'server.socket_host': ip,
        'server.socket_port': port,
        'server.ssl_module': 'pyopenssl',
        'server.ssl_certificate': f"{cert_path}",
        'server.ssl_private_key': f"{key_path}",
        'engine.autoreload_on': True,
        'log.screen': True,
    }


def main():
    os.environ["DJANGO_SETTINGS_MODULE"] = "quiz_craft_backend.settings"
    django.setup()

    ip, port = read_server_confing()
    manager = ssl_m.SSLManager()

    if not manager.certificates_available():
        print(f"For the server to work, you need to provide SSL certificates "
              f"({manager.cert_path} and {manager.key_path}) for '{ip}' or domain "
              f"and place them to {os.path.abspath('system')} folder.\n"
              f"Also you can provide path to certificates in {os.path.abspath('system/ssl.conf')} file.")

        input("Press enter to exit...")
        return

    cherrypy.config.update(cherrypy_configs(ip, port, manager.cert_path, manager.key_path))

    mount_static(settings.STATIC_URL, settings.STATIC_ROOT)

    cherrypy.tree.graft(WSGIHandler())
    call_command("migrate")
    cherrypy.engine.start()

    cherrypy.engine.block()


if __name__ == "__main__":
    main()

class SSLManager:
    def __init__(self):
        self.cert_path, self.key_path = self.read_ssl_config()

    @staticmethod
    def read_ssl_config():
        try:
            file = open("system/ssl.conf")
            cert_path = file.readline().split("=")[1].strip()
            key_path = file.readline().split("=")[1].strip()
            file.close()

            if len(cert_path) == 0 or len(key_path) == 0:
                raise Exception("")

            return cert_path, key_path
        except Exception as e:
            print(e)

        return "system/cert.pem", "system/key.pem"

    def certificates_available(self):
        try:
            cert_ = open(self.cert_path)
            cert_.close()
            key_ = open(self.key_path)
            key_.close()

            return True
        except:
            return False

import socket

terminate_string = '-term-end-'
BUFF_SIZE = 1024

if __name__ == "__main__":
    server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    server_socket.bind(('127.0.0.1', 8080))
    server_socket.listen(10)

    while True:
        (client_socket, server) = server_socket.accept()

        result = ''.encode('utf-8')

        while True:
            data = client_socket.recv(BUFF_SIZE)

            if data.find(terminate_string.encode('utf-8')) != -1:
                data = data.replace(terminate_string.encode('utf-8'), ''.encode('utf-8'))
                result += data
                break

            result += data
            if len(data) < BUFF_SIZE:
                break

        if result.find(terminate_string.encode('utf-8')) != -1:
            result = result.replace(terminate_string.encode('utf-8'), ''.encode('utf-8'))
        print(f'length of result: {len(result)}')

        sendData = f'received {len(result)} bytes' + terminate_string
        send_len = client_socket.send(sendData.encode('utf-8'))

        if send_len != len(sendData):
            print('error on send')

        with open('./4k_image_received.jpg', 'wb') as f:
            f.write(result)

        client_socket.close()
